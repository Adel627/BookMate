using BookMate.web.Extensions;
using BookMate.web.Interfaces;
using BookMate.web.Services;
using BookMate.web.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookMate.web.Controllers
{
    [Authorize(Roles = AppRoles.Reception)]
    public class SubscriberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IEmailBodyBuilder _emailBodyBuilder;
        private readonly IEmailSender _emailSender;
        private readonly ICloudinary _cloudinary;
        private readonly IMapper _mapper;
        public SubscriberController(IUnitOfWork unitOfWork , IEmailBodyBuilder emailBodyBuilder, IEmailSender emailSender,
            IMapper mapper,
            IOptions<CloudinarySettings> cloudinary )
        {
            _unitOfWork = unitOfWork;
            _emailBodyBuilder = emailBodyBuilder;
            _emailSender = emailSender;
            _mapper = mapper;
            Account account = new()
            {
                Cloud = cloudinary.Value.Cloud,
                ApiKey = cloudinary.Value.ApiKey,
                ApiSecret = cloudinary.Value.ApiSecret,
            };
            _cloudinary = new Cloudinary(account);
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task< IActionResult> Search(string searchTerm)
        {
           var subscriber = await _unitOfWork.Subscribers.GetSubscriber(searchTerm);
            return PartialView("_SubscriberDetailsPartialView",subscriber);
        }
        public async Task< IActionResult> Create()
        {
           var model =await _unitOfWork.Subscribers.PopulateViewModel();
            
            return View(model);
        }
        [HttpPost]
        public async Task< IActionResult> Create(SubscriberFormViewModel model)
        {
            //Uniqe email and national id
            if (!ModelState.IsValid)
            {
                model = await _unitOfWork.Subscribers.PopulateViewModel(model);
                return View(model);
            }
            var subscriber = _mapper.Map<Subscriber>(model);

            //Save image
            var extension = Path.GetExtension(model.Image.FileName);

            if (!ImageValid.allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError(nameof(model.Image), Errors.NotAllowedExtension);
            }

            if (model.Image.Length > ImageValid.maxAllowedSize)
            {
                ModelState.AddModelError(nameof(model.Image), Errors.MaxSize);
            }

            var imageName = $"{Guid.NewGuid()}{extension}";

            using var straem = model.Image.OpenReadStream();

            var imageParams = new ImageUploadParams
            {
                File = new FileDescription(imageName, straem),
                UseFilename = true // force cloudinary to use the name i send
            };

            var Result = await _cloudinary.UploadAsync(imageParams);

            subscriber.ImageUrl = Result.SecureUrl.ToString();
            subscriber.ImageThumbnailUrl = GetThumbnailUrl(subscriber.ImageUrl);
            subscriber.ImagePublicId = Result.PublicId;
            subscriber.CreatedById = User.GetUserId();

            Subscription subscription = new()
            {
                CreatedById = subscriber.CreatedById,
                CreatedOn = subscriber.CreatedOn,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1)
            };

            subscriber.Subscriptions.Add(subscription);
            await _unitOfWork.Subscribers.AddAsync(subscriber);
            await _unitOfWork.CompleteAsync();
            //add welcom email
            var placeholders = new Dictionary<string, string>()
            {
                { "imageUrl", "https://res.cloudinary.com/ddkthlwge/image/upload/v1762406863/unnamed_keadn4.jpg" },
                { "header", $"Welcome {model.FirstName}," },
                { "body", "thanks for joining Bookify 🤩" }
            };

            var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Notification, placeholders);

              await _emailSender.SendEmailAsync( model.Email,"Welcome to Bookify", body);

            //Send welcome message using WhatsApp


            return RedirectToAction("Index");
        }


        
        public async Task<IActionResult> RenewSubscription(int subscriberId)
        {
          var subscriber = await _unitOfWork.Subscribers.GetByIdAsync(subscriberId);
            if (subscriber is null)
                return NotFound();

            if (subscriber.IsBlackListed)
                return BadRequest();


            var endDate = _unitOfWork.Subscribers.RenewSubscription(subscriberId , User.GetUserId());

            //Send email 
            var placeholders = new Dictionary<string, string>()
            {
                { "imageUrl", "https://res.cloudinary.com/ddkthlwge/image/upload/v1762406863/unnamed_keadn4.jpg" },
                { "header", $"Hello {subscriber.FirstName}," },
                { "body", $"your subscription has been renewed through {endDate.ToString("d MMM, yyyy")} 🎉🎉" }
            };

            var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Notification, placeholders);

            await _emailSender.SendEmailAsync(
                subscriber.Email,
                "Bookify Subscription Renewal", body);

            //Send WhatsApp Message


            return RedirectToAction("Index");
        }

        public async Task< IActionResult> GetAreas(int id)
        {
            var areas = await _unitOfWork.Subscribers.GetGovernorateAreas(id);
            return Json(areas);
        }
        public IActionResult AllowNationalId(string NationalId)
        {
            
            return Json(_unitOfWork.Subscribers.AllowNationalId(NationalId));
        }

        public IActionResult AllowMobileNumber(string MobileNumber)
        {
            return Json(_unitOfWork.Subscribers.AllowMobileNumber(MobileNumber));

        }

        public IActionResult AllowEmail(string Email)
        {
            return Json(_unitOfWork.Subscribers.AllowEmail(Email));
        }

        private string GetThumbnailUrl(string url)
        {
            var separator = "image/upload/";
            var urlParts = url.Split(separator);

            var thumbnailUrl = $"{urlParts[0]}{separator}c_thumb,w_200,g_face/{urlParts[1]}";

            return thumbnailUrl;
        }
    }
}
