# Reactivities
Social media website created using C#, .NET, React(typescript), sqlite database was used for development and postgres for production.
This project was done with mediator design pattern and CQRS. 
# Preview
Main page:
![preview1](https://github.com/gotham237/Reactivities/assets/86183687/90935509-b01c-4771-88fa-b336ee6f121c)

Activity:
![image](https://github.com/gotham237/Reactivities/assets/86183687/bbd2ea9f-b3a4-4175-840d-c8d91aae1afe)

Profile:
![image](https://github.com/gotham237/Reactivities/assets/86183687/1c3f4e7a-82b2-429b-8028-426b6e11ff5e)

Profile -> Photos:
![image](https://github.com/gotham237/Reactivities/assets/86183687/930c06bc-c667-4d16-96ca-4501a5516817)

Profile -> Events:
![image](https://github.com/gotham237/Reactivities/assets/86183687/97bb520e-043f-4341-8e00-be27df1475ad)

Profile -> Followers:
![image](https://github.com/gotham237/Reactivities/assets/86183687/f1186abf-1fed-4fe7-a85a-6ba34374c8dd)

Profile -> Followings:
![image](https://github.com/gotham237/Reactivities/assets/86183687/61105aa8-da47-4ec9-92ab-4dabb930e508)

# Features:
- login, register (JWT),
- create, edit, attend, cancel activities,
- send messages through chat in every activity (made with signalR web sockets)
- follow/unfollow other users,
- upload photos(photos are stored in cloud service - Cloudinary),
- update profile info, check events: future events, past events, and events that you are hosting, see followers and users that you are following
- sorting events: "All Activities", "I'm going", "I'm hosting"
# Deployment
I used docker, fly.io free service and github actions for automatic deployment.
# React 
Used: 
 - typescript,
 - semantic ui,
 - formik for forms creation, Yup for form validation,
 - mobx,
and more.
