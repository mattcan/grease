# Grease Project

## Grease

Grease is a syntax for creating assessments that is heavily based on Markdown.

### What does it look like?

```md
# Grease Project Pop Quiz

1. What is Grease?
    * [x] A Domain Specific Language for creating assessments
    * [x] Super cool
    * [ ] Not this project

1. Does it look a lot like Markdown?
    * ( ) No
    * (x) Yes
```

## Swivel

Swivel is a command line application for parsing Grease files into JSON or HTML.

![Picture of Grease assessment rendered to HTML](demo/parsed_html.png)

## Installation

Currently there isn't an installer or single executable. To build the project, run the following commands:

```sh
cd src/Swivel
dotnet build -c Release
sudo ln -s ~/grease/src/Swivel/bin/Release/netcoreapp2.0/ubuntu.16.04-x64/Swivel /usr/bin/swivel
```

### Usage

```sh
swivel my_quiz.md my_quiz_output.html --format=html
```
