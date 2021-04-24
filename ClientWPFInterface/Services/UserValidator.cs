using ClientWPFInterface.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPFInterface.Services
{
    public class UserValidator : AbstractValidator<WindowModel>
    {
        public UserValidator()
        {
            RuleFor(model => model.NameUser)
                .NotEmpty().WithMessage("Поле не може бути \nпустим!");
        }
    }
}
