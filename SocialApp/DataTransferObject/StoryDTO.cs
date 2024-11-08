﻿using SocialApp.Models;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.DataTransferObject
{
    public class StoryDTO
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; } = DateTime.Now.ToUniversalTime();

        public bool IsHidden { get; set; }

        [DataType(DataType.Url)]
        public string? ImgURL { get; set; }

        public IFormFile? Img { get; set; }

        public int LikesCount { get; set; } = 0;

        public string? AuthorProfileId { get; set; }

        public StoryDTO() { }

        //Copy constructor
        public StoryDTO(Story story)
        {
            DateTime = story.DateTime;
            IsHidden = story.IsHidden;
            ImgURL = story.ImgURL;
            LikesCount = story.LikesCount;
            AuthorProfileId = story.AuthorProfileId;

        }
    }
}
