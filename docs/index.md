# Quizlet Project Documentation

## 1. Introduction and Goals

Quizlet is a web-based application that allows users to create, study, and test their knowledge through flashcards and quizzes. The system leverages AI (OpenAI) to generate flashcards from user-uploaded text, providing an enhanced learning experience.

## 2. Constraints

- Frontend: Vue 3 + Vite
- Backend: .NET 9
- Integration with OpenAI API for flashcard generation
- Support for multiple file types (PDF, TXT, DOCX)

## 3. System Scope and Context

### 3.1 Business Context

```mermaid
C4Context
    title System Context diagram for Quizlet

    Person(user, "User", "A user who wants to learn and test their knowledge")
    
    System(quizlet, "Quizlet System", "Allows users to create, study, and test their knowledge through flashcards and quizzes")
    
    System_Ext(openai, "OpenAI", "Hosted LLM to generate Flashcards from Text.")
    
    Rel(user, quizlet, "Uploads text, generates cards, learns from cards.", "HTTPS")
    Rel(quizlet, openai, "Generates Flashcards using", "HTTPS/API")
    
    UpdateLayoutConfig($c4ShapeInRow="1", $c4BoundaryInRow="1")
```

**Explanation:**  
This context diagram illustrates Quizlet's core interactions: Users interact with the Quizlet system through a web interface, while the system itself leverages OpenAI's language model capabilities to automatically generate flashcards from text input. The diagram emphasizes that Quizlet is primarily a user-facing application with an AI-powered content generation feature.

### 3.2 Technical Context

- Users access the system via HTTPS.
- The system communicates with OpenAI via HTTPS/API.

## 4. Solution Strategy

- Modular architecture with clear separation between frontend and backend.
- Use of strategy pattern for file reading to support multiple formats.
- Integration with external AI service (OpenAI) for content generation.

## 5. Building Block View

### 5.1 Level 1: Container Diagram

```mermaid
C4Container
    title Container diagram for Quizlet

    Person(user, "User")

    System_Boundary(quizlet_system, "Quizlet") {
        Container(frontend, "Frontend", "Vue 3 + Vite", "Handles user interaction and communicates with the backend.")
        Container(api, "Backend", ".NET 9", "Processes user input, handles file parsing, and calls the OpenAI API to generate flashcards.")
    }

    System_Ext(openai, "OpenAI", "External LLM for generating flashcards")

    Rel(user, frontend, "Uses", "HTTPS")
    Rel(frontend, api, "Sends quiz creation requests", "REST")
    Rel(api, openai, "Sends content to generate flashcards", "HTTPS API")

    UpdateLayoutConfig($c4ShapeInRow="2", $c4BoundaryInRow="1")
```

**Explanation:**  
The frontend, built with Vue 3 and Vite, handles user interaction including uploading documents and initiating quiz creation. It communicates with a .NET 9 backend, which processes the uploaded files, extracts content using file readers, and communicates with the OpenAI API to generate quiz questions.

### 5.2 Level 2: Component Diagram

```mermaid
graph TD
    User[User]
    QuizController[QuizController<br>ASP.NET Core Controller]
    FileService[FileService<br>Handles file upload and preprocessing]
    CardDeckService[CardDeckGenerationServiceOpenAI<br>Uses OpenAI API]
    FileReaders[FileReaderStrategy<br>PDF, TXT, DOCX Readers]
    OpenAI[(OpenAI API)]

    User --> QuizController
    QuizController --> FileService
    QuizController --> CardDeckService
    FileService --> FileReaders
    CardDeckService --> OpenAI
```

**Explanation:**  
This diagram provides a deeper look inside the backend container. User requests are routed through the QuizController, which delegates file handling to the FileService and quiz generation to CardDeckGenerationServiceOpenAI. The FileService uses a strategy pattern to handle multiple file types (PDF, TXT, DOCX) via pluggable readers. The CardDeckGenerationServiceOpenAI integrates with the OpenAI API to generate flashcards based on the parsed content.

## 6. Runtime View

- User uploads a document via the frontend.
- Frontend sends the file to the backend.
- Backend processes the file, extracts text, and sends it to OpenAI.
- OpenAI returns generated flashcards, which are sent back to the frontend for user interaction.

## 7. Deployment View

- Frontend and backend are deployed independently.
- Backend requires network access to OpenAI API.

## 8. Cross-cutting Concepts

- Security: HTTPS for all communications.
- Extensibility: Strategy pattern for file readers.
- Scalability: Frontend and backend can be scaled independently.

## 9. Design Decisions
An architecture descision record can be found [here](./adr.md)