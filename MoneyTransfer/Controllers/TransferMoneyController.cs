using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoneyTransfer.BLL.Dtos;
using MoneyTransfer.BLL.Interfaces;
using MoneyTransfer.BLL.Services;
using MoneyTransfer.Data.Entites;

namespace MoneyTransfer.Presentation.Controllers
{
    public class TransferMoneyController : Controller
    {
        private readonly ITransactionInformationService _transactionInformation;
        private readonly CountryService _countryService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;

        public TransferMoneyController(ITransactionInformationService transactionInformation, CountryService countryService, SignInManager<AppUser> signInManager, IMapper mapper)
        {
            _transactionInformation = transactionInformation;
            _countryService = countryService;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [Authorize]
        public IActionResult Transfer()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "CheckCurrency");
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(PaymentDetails payment)
        {
            TransactionInformationDto dto = new();
            dto.PaymentDetails = payment;
            dto.countryList = await _countryService.CountryList();
            return View("Transfer", dto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateTransaction(TransactionInformationDto dto)
        {
            var claim = User.Claims.First(c => c.Type == "UserId");
            dto.UserId = claim.Value;
            var user = await _signInManager.UserManager.FindByIdAsync(dto.UserId!);
            var userMapper = _mapper.Map<SenderDetails>(user);
            dto.SenderDetails = userMapper;
            dto.countryList = await _countryService.CountryList();

            if (ModelState.IsValid)
            {
                var result = await _transactionInformation.CreateTransactionInformation(dto);
                if (result.Success)
                {
                    return RedirectToAction("Details", "TransactionInformation", result.Data);
                }
                return View("Transfer", dto);
            }
            else
            {
                return View("Transfer", dto);
            }
        }
    }
}
