# Quizlet Project Documentation

## System Context Diagram

A C4 context diagram is the highest level of abstraction in the C4 model, providing a big picture view of how your software system interacts with users and other systems. It shows the system as a single box, surrounded by its users and any external systems it interacts with. This level of the C4 model is particularly useful for explaining the system to non-technical stakeholders and understanding the system's scope and boundaries.

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

This context diagram illustrates Quizlet's core interactions: Users interact with the Quizlet system through a web interface, while the system itself leverages OpenAI's language model capabilities to automatically generate flashcards from text input. The diagram emphasizes that Quizlet is primarily a user-facing application with an AI-powered content generation feature, making it clear that the system's main value proposition is the combination of user interaction and AI-assisted content creation.

## Container Diagram
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
This container diagram breaks down the Quizlet system into its primary executable parts. The frontend, built with Vue 3 and Vite, handles user interaction including uploading documents and initiating quiz creation. It communicates with a .NET 9 backend, which processes the uploaded files, extracts content using file readers, and communicates with the OpenAI API to generate quiz questions. This separation of concerns allows for modular development and independent scaling of components. The diagram helps developers and architects understand the responsibilities and interactions of each container.

## Component Diagram

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
The component diagram provides a deeper look inside the backend container. It illustrates how user requests are routed through the QuizController, which delegates file handling to the FileService and quiz generation to CardDeckGenerationServiceOpenAI. The FileService uses a strategy pattern to handle multiple file types (PDF, TXT, DOCX) via pluggable readers. The CardDeckGenerationServiceOpenAI integrates with the OpenAI API to generate flashcards based on the parsed content. This design promotes extensibility, testability, and separation of concerns in the backend architecture.
