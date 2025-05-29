
-- Table Member (Utilisateur)
CREATE TABLE Member (
    Member_ID INT IDENTITY(1,1) PRIMARY KEY,
    UserName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    CreationDate Date NOT NULL,
    IsOnline BIT DEFAULT 0
);

-- Table Subreddit (Blogue)
CREATE TABLE Blogue (
    Blogue_ID INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(200) NOT NULL,
    Description TEXT,
    CreationDate DATE NOT NULL,
    Categorie VARCHAR(255),
    Member_ID INT,
    FOREIGN KEY (Member_ID) REFERENCES Member(Member_ID)
);

-- Table Post (Article)
CREATE TABLE Post (
    Post_ID INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(200) NOT NULL,
    Content TEXT NOT NULL,
    PublishingDate DATE NOT NULL,
    Post_URL VARCHAR(255),
    Blogue_ID INT,
    Member_ID INT,
    FOREIGN KEY (Blogue_ID) REFERENCES Blogue(Blogue_ID),
    FOREIGN KEY (Member_ID) REFERENCES Member(Member_ID)
);

-- Table Comment (Commentaire)
CREATE TABLE Comment (
    Comment_ID INT IDENTITY(1,1) PRIMARY KEY,
    Content TEXT NOT NULL,
    PublishingDate DATE NOT NULL,
    Comment_URL VARCHAR(150),
    Post_ID INT,
    Member_ID INT,
    FOREIGN KEY (Post_ID) REFERENCES Post(Post_ID),
    FOREIGN KEY (Member_ID) REFERENCES Member(Member_ID)
);

CREATE TABLE CommentLike (
   Like_ID INT IDENTITY(1,1) PRIMARY KEY,
   Comment_ID INT,
   Member_ID INT
   FOREIGN KEY (Comment_ID) REFERENCES Comment(Comment_ID),
   FOREIGN KEY (Member_ID) REFERENCES Member(Member_ID)
);


CREATE TABLE CommentDislike (
   DisLike_ID INT IDENTITY(1,1) PRIMARY KEY,
   Comment_ID INT,
   Member_ID INT
   FOREIGN KEY (Comment_ID) REFERENCES Comment(Comment_ID),
   FOREIGN KEY (Member_ID) REFERENCES Member(Member_ID)
);

CREATE TABLE PostLike (
   Like_ID INT IDENTITY(1,1) PRIMARY KEY,
   Post_ID INT,
   Member_ID INT
   FOREIGN KEY (Post_ID) REFERENCES Post(Post_ID),
   FOREIGN KEY (Member_ID) REFERENCES Member(Member_ID)
);


CREATE TABLE PostDislike (
   DisLike_ID INT IDENTITY(1,1) PRIMARY KEY,
   Post_ID INT,
   Member_ID INT
   FOREIGN KEY (Post_ID) REFERENCES Post(Post_ID),
   FOREIGN KEY (Member_ID) REFERENCES Member(Member_ID)
);
