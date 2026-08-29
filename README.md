# Voclipy

🔴 Live Demonstration: https://voclipy.ru

Voclip is a web service for learning English through a personal vocabulary dictionary and YouTube content. The main idea of the project is that users build their own collection of English words and expressions while studying and then reinforce them using an integrated training system.

The service consists of four main modules:
 - Personal Dictionary
 - YouTube Learning
 - Training
 - User Profile

### Personal Dictionary
#Screenshot: [dictionary](image/dictionary.png)
Users can build their own collection of English words and expressions for future learning. A dictionary note can contain:
- Individual words
- Phrasal verbs
- Idiomatic expressions
- Grammar constructions

Each note can store:
- One or more translations
- One or more example sentences
- A learning source

The learning source can be:
- Context extracted from a YouTube video

### Learning with YouTube
#Screenshot: [video](image/video.png) и [player](image/player.png)
The service provides a collection of short YouTube videos for learning English in a real-world context. While watching a video, users can save new words and expressions directly to their personal dictionary.
If a word already exists in the user's dictionary, it is highlighted inside the subtitles. This allows users to learn vocabulary in context instead of studying isolated words.

### Training
#Screenshot: [train](image/train.png)
After adding words to the dictionary, users can reinforce their knowledge through training exercises.
Each dictionary note has a learning score:
- The score increases after a correct answer.
- The score decreases after an incorrect answer
The lower the score, the more frequently the word appears during training.

### User Profile
#Screenshot: [profile](image/profile.png)
The user profile provides personal information and learning statistics. It includes the following features:
- Uploading and changing the profile avatar
- Viewing the total number of saved words
- Viewing the current dictionary level
- Viewing the total number of videos available in the service
- Viewing the user's latest activity (for example, recently added or recently trained words)

### Authentication
All functionality is available only to authenticated users.
Registration and sign-in are performed using email verification. Users enter their email address and receive a one-time verification code to access the service.

## Technology Stack
Backend
- C#
- ASP.NET CORE 8
- Clean Architecture
- CQRS
- MediatR
- Repository Pattern
- Unit of Work
- Domain Driven Design (Entities, ValueObjects, Factory Methods)
- Entity Framework Core
- Fluent Validation
- PostgreSQL
- Redis
- JWT
- xUnit(Unit tests)
- S3 Storage (Supabase)
- LLM (Groq Api, OpenRouter Api)
- Youtube Data Api
- MyMemory Api (translation service)
- Resend Api (send email)

Frontend
- JS
- React
- React Router
- Axios Api
- Redux Toolkit
- CSS Modules

DevOps
- Docker
- Docker compose
- Nginx
- Linux VPS
- Git
- env (stored on vps server)


