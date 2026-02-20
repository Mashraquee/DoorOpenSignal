# DoorOpenSignal

System State: Defined with an enum (RUNNING, MAINTENANCE).

Game Loop: Simulated with a Task that prints "Game loop running..." every second.

Signal Handling: When user types signal door_open, the system:

Switches to MAINTENANCE.

Logs "Door opened – entering maintenance mode."

Cancels the game loop via CancellationToken.

This way, you can simulate both the alert logging and the game loop termination in a clean, testable manner.
