using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FptHrLearningSystem.Models
{
    public enum PerferredCampus { PPT, NX, CH }
    public enum Gender { Male, Female, Other }
    public enum Semester { Spring, Summer, Fall }
    public enum TrainerType { Internal, External }
    public enum Role { Admin, Staff, Trainer, Trainee }
    
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
    public class Course
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Name_EN { get; set; }
        public string Name_VI { get; set; }
        public int? Credit { get; set; }
        public int? Hour { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Classroom> Classrooms { get; set; }
        public virtual ICollection<UserCourse> UserCourses { get; set; }
        public virtual ICollection<TrainerCourse> TrainerCourses { get; set; }
    }
    
    public class ClassProfile
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? EnrolmentYear { get; set; }
        public PerferredCampus? PerferredCampus { get; set; }
        public virtual ICollection<Classroom> Classrooms { get; set; }
    }
    public class Classroom
    {
        public int Id { get; set; }
        public string Code { get; set; }
        [Required]
        [UIHint("StartDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public Semester Semester { get; set; }
        [Required]
        public int Part { get; set; }
        public int? ClassProfileId { get; set; }
        public virtual ClassProfile ClassProfile { get; set; }
        [Required]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<TraineeClassroom> TraineeClassrooms { get; set; }
        
    }
    public class UserCourse
    {
        [Key]
        [Column(Order =0)]
        [Required]
        public string UserId { get; set; }
        [Required]
        [Key]
        [Column(Order = 1)]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public virtual ApplicationUser User { get; set; }
    }



    public class TraineeClassroom
    {
        public int Id { get; set; }
        [Required]
        public int ClassroomId { get; set; }
        public virtual Classroom Classroom { get; set; }
        [Required]
        public string TraineeId { get; set; }
        public virtual ApplicationUser Trainee { get; set; }
    }
    public class TrainerCourse
    {
        public int Id { get; set; }
        [Required]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        [Required]
        public string TrainerId { get; set; }
        public virtual ApplicationUser Trainer { get; set; }
    }
   
    public class FptHr
    {
    }
}