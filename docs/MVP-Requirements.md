# MVP Requirements

I write this file to have a concrete Goal to build towards.  
To understand the full scope of what I will be building please read my [design decisions](Design-Decisions.md).  

The Targets written down here are just priorities and restrictions for the ["Minimum viable product"](https://en.wikipedia.org/wiki/Minimum_viable_product) of my application.  

### Target 1

I want it bare bones!  

- As few dependencies as possible  
- No fancy logging abstractions
- As few distractions as possible

### Target 2

In my MVP it is obvious that not every functionality can be built out to the full extent.  
However it is important to me, that the following features work with reasonable UX:

- Uploading a new package
- Downloading a package
- Delete a package
- Listing all packages

And that's it. Everything else is bonus and should be considered to leave out for now.  

### Target 3

Basic User management  

Because this application will live on public VPS, there should be basic user management and Auth in place.  
But allow me to circle back to [Target 1](MVP-Requirements.md#target-1):  

- No framework
- No fancy package
- No external service

Just a simple DB storing usernames and passwords.  
No tokens for now.  

### Target 4

Automatic deployment.  
I want to use Docker Stack Deploy and a CI/CD Pipeline.  

This is important for me to do now, because it's good practise for me and easier to do in the beginning.   
