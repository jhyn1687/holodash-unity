# HoloDash Unity Git Tracker

## Getting Started

Install [Git LFS](https://git-lfs.com/) just in case.

>git lfs install

## Unity

Create a project with 2D (URP). Then git clone the repo somewhere else and copy all the folders in.

## What's in this "base"?

* Basic movement, physics, dashing
* A basic tile palette.
* Player sprite w/ animation
* Basic weapon shooting
* Enemies with HP, basic sprite.

## Variables to keep consistent

![image](https://user-images.githubusercontent.com/25493737/215223273-b156dd4a-16e8-4326-8d2a-6502872d6863.png)

Project Settings > Physics 2D > Gravity > Y = -30 

## Make a branch and add stuff!

How to branch and push to remote:
```
git checkout -b <name>
git add .
git commit -m "<message>"
git push -u origin <name>
```

How to fast forward back to main on your branch (note: this will override your remote and local branch):
```
git checkout main
git branch -D <name>
git checkout -b <name>
git push -u origin <name>
```
