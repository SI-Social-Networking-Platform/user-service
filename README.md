# User Service

The **User Service** manages user authentication, registration, and social features like following users.

## Controllers

### 1. AuthController

This controller handles user registration and login. It also generates JWT tokens to secure other API endpoints.

- **Endpoints:**

  - `POST /user-service/auth/register`
    - **Description:** Registers a new user using a username, email, and password. Returns a JWT token when successfully registrated.
    - **Request Body:** 
      - `RegisterRequest` with properties:
        - `Username`: The desired username.
        - `Email`: The user's email address.
        - `Password`: The user's password.
    - **Response Codes:**
      - `201`: Successfully created the user and returned a JWT token.
      - `400`: Bad request, user creation failed.

  - `POST /user-service/auth/login`
    - **Description:** Authenticates a user by their username and password. Returns a JWT token if authentication is successful.
    - **Request Body:** 
      - `LoginRequest` with properties:
        - `Username`: The username of the user trying to log in.
        - `Password`: The user's password.
    - **Response Codes:**
      - `200`: Successfully authenticated the user and returned a JWT token.
      - `401`: Unauthorized, invalid credentials.

### 2. UsersController

Manages user follow actions, searches, and user data retrieval.

- **Endpoints:**

  - `POST /user-service/follow`
    - **Description:** Allows an authenticated user to follow another user by their ID.
    - **Authorization:** Requires a valid JWT token.
    - **Request Body:** An integer representing the followed user's ID.
    - **Response Codes:**
      - `200`: Successfully followed the user.
      - `400`: Unable to follow, possibly already following or user not found.

  - `GET /user-service/{id}/follows`
    - **Description:** Retrieves a list of user IDs that the user with the specified ID is following.
    - **Path Parameter:** `id` representing the user's unique identifier.
    - **Response Codes:**
      - `200`: Returns a list of user IDs being followed.
      - `404`: User not following anyone.

  - `GET /user-service/user/{id}`
    - **Description:** Retrieves user information by their unique ID.
    - **Path Parameter:** `id` representing the user's unique identifier.
    - **Response Codes:**
      - `200`: Returns the user's information.
      - `404`: User not found.

  - `GET /user-service/search`
    - **Description:** Searches for users based on a given name or username.
    - **Query Parameter:** `name` containing the partial or full name to search for.
    - **Response Codes:**
      - `200`: Returns a list of matching users.
      - `404`: No matching users found.

## Security

- **JWT Authentication:** The service uses JWT tokens for securing the endpoints that require authorization.

