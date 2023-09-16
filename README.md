# Reactivities
Social media website created using C#, .NET, React(typescript), sqlite database was used for development and postgres for production.
This project was done with mediator design pattern and CQRS. 
# Features:
- login, register (JWT),
- create, edit, attend activities,
- send messages through chat in every activity (made with signalR web sockets)
- follow other users,
- upload photos(photos are stored in cloud service - Cloudinary),
- update profile info, check events: future events, past events, and events that he is hosting, see followers and users that he is following

# Deployment
I used docker, fly.io free service and github actions for automatic deployment.

# React 
Used: 
 - typescript,
 - semantic ui,
 - formik for forms creation, Yup for form validation,
 - mobx,
and more.
