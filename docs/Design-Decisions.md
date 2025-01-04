# Design Decisions

What is this supposed to be?

TheMover is the current name for a program I am developing, which mainly focuses on providing files for a user like a clipboard.  
The point of it is, to have files persisted through system reboots, in the cloud and across multiple devices.   

TheMover will be split into multiple smaller projects where I will get to experiment.  
The API project will only revolve around server side implementations and business logic.  
Every Client in the future will depend on this.    

The goal is to have a docker container that will be hosted on a VPS with the possibility to self-host.  

The things you find here are my initial thoughts on how I want to start this.  
This file will transform over time, and the record of it changing, will be found in the git repository, as well as in the ADR directory.  
Click [here](adr/readme.md) to find out about the ADR.    

## Language

I choose C# for this because I always wanted to do some server side gRPC in C#.  
Also performance is not a priority with this and I have the most experience in C#.  

## API

My API will be powered by gRPC for type safety and intuitive and well documented client-server communication.  
Files will be served via a minimal API for performance reasons (gRPC is NOT good at file serving)  

## Messaging

Some parts of my application will require messaging.  
For that reason I will integrate [RabbitMQ](https://www.rabbitmq.com/) into my app.

It will be responsible for asynchronous API's once the need arises and also to keep all my Clients in Sync.  

## Database
[//]: # ( Todo: What DB will I use? )
No yet decided.

## Analytics + Logging
[//]: # ( Todo: What will I use for this? [Maybe look into Grafana and Prometheus] )

No yet decided.  

## Deployment

The application will be fully dockerized and deployed via docker stack deploy.  
My Dockerfiles and my stack deploy files will be completely written by hand.  

The code gen products are not developed far enough for my needs.  
(There are solutions for Kubernetes like [aspirate](https://github.com/prom3theu5/aspirational-manifests) but nothing for docker stack deploy)  

## Things I have decided against

### .Net Aspire (on prod)

This was an Idea I had in the beginning. I wanted to do this for observability reasons since I quite liked the dashboard.  
Sadly I noticed that it was just not meant to be deployed on prod...  

I may use it for superior local debugging experience, because that is what its actually meant for.  
But I have accepted now, that my fist idea, to host an aspire dashboard via docker and configuring it via environment Variables, is just not it.  
It's just too much effort for now.  

### Generating deployment with [aspirate](https://github.com/prom3theu5/aspirational-manifests)

In the [Deployment](Design-Decisions.md#deployment) chapter I said that I decided against using [aspirate](https://github.com/prom3theu5/aspirational-manifests) to generate my deployments.  
I say that because until now I could not make it output the code I wanted with .Net Aspire.  
Also there are currently no automations except for azure containers, which I don't want to use for this project.  

This may change in the future.  
If there exists a new Tool that is made for docker stack deploy and works well with GitHub Actions, I will be thrilled to try it out.  
