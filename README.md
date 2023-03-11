# HoloDash Unity Git Repository

## Getting Started

Install [Git LFS](https://git-lfs.com/) just in case.

>git lfs install

## Unity

Using Unity version 2021.3.17f1 LTS

Git clone the repo and open with Unity (all the dependencies should be installed automatically by Unity.

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
