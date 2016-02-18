﻿namespace H8QMedia.Web.ViewModels.Course
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using H8QMedia.Data.Common;
    using H8QMedia.Data.Models;
    using H8QMedia.Web.Infrastructure.Mapping;
    using H8QMedia.Web.ViewModels.Comment;
    using H8QMedia.Web.ViewModels.Image;
    using H8QMedia.Web.ViewModels.User;

    public class CourseViewModel : IMapFrom<Course>, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Required]
        [MinLength(
            ValidationConstants.MinInteractiveEntityTitleLength,
            ErrorMessage = ValidationConstants.MinLengthErrorMessage)]
        [MaxLength(
            ValidationConstants.MaxInteractiveEntityTitleLength,
            ErrorMessage = ValidationConstants.MaxLengthErrorMessage)]
        public string Title { get; set; }

        [MaxLength(
            ValidationConstants.MaxInteractiveEntityDescriptionLength,
            ErrorMessage = ValidationConstants.MaxLengthErrorMessage)]
        public string Description { get; set; }

        public virtual ICollection<CommentViewModel> Comments { get; set; }

        public int LikesCount { get; set; }

        public virtual ICollection<ImageViewModel> Images { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ICollection<UserViewModel> Students { get; set; }

        public ICollection<UserViewModel> Trainers { get; set; }

        public ICollection<CourseObjective> Objectives { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Course, CourseViewModel>()
               .ForMember(x => x.LikesCount, opt => opt.MapFrom(x => x.Likes.Any() ? x.Likes.Count() : 0));
        }
    }
}
