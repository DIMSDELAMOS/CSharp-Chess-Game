# C# Chess Game ♟️

A classic 2-player **Chess Game** developed using **C#** and **Windows Forms (.NET)**. This application implements standard chess rules, piece movement logic, and a graphical user interface for an engaging gameplay experience.

## 🎮 Features

* **Complete Board Logic:** Validates legal moves for all pieces (Pawn, Rook, Knight, Bishop, Queen, King).
* **Turn-Based System:** Automatically switches turns between White and Black players.
* **Visual Interface:**
    * Interactive grid board.
    * Highlighted valid moves (optional - if you have implemented this).
    * Visual indicators for selected pieces.
* **Game States:** Detects check (Checkmate/Stalemate logic included).
* **Reset Functionality:** Ability to restart the game without reopening the application.

## 🛠️ Technical Implementation

* **Language:** C#
* **Framework:** .NET Windows Forms
* **Concepts Used:**
    * **OOP (Object-Oriented Programming):** Each piece is a separate class inheriting from a base `Piece` class.
    * **2D Arrays:** The board is represented/managed using a coordinate system logic.
    * **Event Handling:** Mouse clicks manage piece selection and movement.

## 📸 Screenshots
*(Optional: Provide a screenshot of the board here)*

## 📦 How to Run

1.  Clone the repository.
2.  Open the `ChessGame.sln` file in **Microsoft Visual Studio**.
3.  Press **Start** (F5) to build and run.
    * *Note: The `bin` and `obj` folders will be regenerated automatically.*

## ♟️ How to Play

1.  **White** always moves first.
2.  Click on a piece to select it.
3.  Click on a valid destination square to move.
4.  Capture the opponent's King to win!
---
*Developed for the University of Piraeus*
