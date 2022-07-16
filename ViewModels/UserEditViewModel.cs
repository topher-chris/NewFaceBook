using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewFaceBook.ViewModels
{
    public class UserEditViewModel : UserCreateViewModel
    {
        public int Id { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
