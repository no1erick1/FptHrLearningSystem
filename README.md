# Fpt Hr Learning System
![GitHub last commit](https://img.shields.io/github/last-commit/ThanhDuong/FptHrLearningSystem)

As the technology is being developed rapidly nowadays, FPT Co. desires to build the continuing study environment throughout the corporation. It is necessary to develop a web-based system, which manages the activity of “Training” for internal training program of the company. This system can be used to manage trainee accounts, manage trainers, manage course categories, manage courses, assign trainer to topic, assign trainee to course. This is a system used by HR department. We have four roles in this system, an administrator, training staff, a trainer and a trainee. The brief description of those roles is as follow
## Migration
From the Tools menu, select NuGet Package Manager > Package Manager Console.
At the ```PM>``` prompt enter the following commands:
```bash
enable-migrations
add-migration InitialCreate
```
## Update-database
```bash
update-database
```
## Main Permission Account In My Project
Permission | Administrator | Training Staff | Trainer | Trainee 
--- | --- | --- | --- |--- 
Account | admin@fpt.edu.vn | admin@fpt.edu.vn | admin@fpt.edu.vn | admin@fpt.edu.vn 
Password | Admin2812@ | Staff2812@ | Trainer2812@ | Trainee2812@ 
## Permission Details Of My Project
An Admin’s role
- Can login to the system through the first page of the application
- Can create/edit/delete new Trainer account and change (if existing user) its password
- Can create/edit/delete new Training Staff account and change (if existing user) its password
A Training Staff’s role
- Can create trainee accounts by entering details.
- Can search a Trainee-by-trainee account, programming language, TOEIC score.
- Can update, delete trainee accounts information
- Can manage course categories such as searching, adding, updating and deleting course categories.
- Can manage courses such as searching, adding, updating and deleting courses. 
- Can manage trainer profile such as adding, updating and deleting the information
- Can assign trainer to a course.
- Can remove courses from Trainer
- Can change Trainer assigned to another course
- Can assign trainee to a course.
- Can Remove courses from Trainee
- Can change Trainee assigned to another course
A Trainer’s role
- In the same system, the trainer who have been registered by the administrator can login and can
update his profile.
- Can update his / her account password
- Can view courses which he is assigned to.
A Trainee’s role
- In the same system, the trainee who have been registered by the administrator can login and can view
his/her profile such
- Can update his / her account password
- Can see all available courses
- Can view courses which s/he is assigned to.
## Deploy
- [Details in here](https://docs.microsoft.com/en-us/aspnet/web-forms/overview/deployment/visual-studio-web-deployment/deploying-to-iis)
## Usage
- [Visual Studio 2019](https://visualstudio.microsoft.com/)
- [SQL Sever](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- JQuery, Html5, Bootstrap 4, Fontawesome 4, C#, Entity framework, Ajax, etc.
- ASP.NET Framwork MVC 5
