﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;



namespace SocialApp.Models
{
    public class AppUser : IdentityUser
    {   //Doesnt need foregin key   
        public UserProfile? UserProfile { get; set; }

        public AppUser()
        {
            // Default constructor
        }

    }
}
