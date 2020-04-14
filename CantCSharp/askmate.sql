DROP TABLE IF EXISTS question_tag;
DROP TABLE IF EXISTS tag;
DROP TABLE IF EXISTS askmate_question_comment;
DROP TABLE IF EXISTS askmate_answer_comment;
DROP TABLE IF EXISTS answer;
DROP TABLE IF EXISTS question;
DROP TABLE IF EXISTS users;

CREATE TABLE users(
userID SERIAL PRIMARY KEY,
username TEXT NOT NULL,
user_password TEXT NOT NULL,
registration_time TIMESTAMP NOT NULL,
email TEXT NOT NULL,
reputation INT
);

CREATE TABLE question(
question_id SERIAL PRIMARY KEY,
submission_time TIMESTAMP,
view_number INT,
vote_number INT,
question_title TEXT,
question_message TEXT,
question_image TEXT,
question_username TEXT,
userID INT REFERENCES users(userID) ON DELETE CASCADE
);
CREATE TABLE answer(
answer_id SERIAL PRIMARY KEY,
submission_time TIMESTAMP,
vote_number INT,
question_id INT REFERENCES question(question_id) ON DELETE CASCADE,
answer_message TEXT,
answer_image TEXT,
answer_username TEXT,
userID INT REFERENCES users(userID) ON DELETE CASCADE
);
CREATE TABLE tag(
tag_id SERIAL PRIMARY KEY,
tag_name TEXT,
UNIQUE (tag_name)
);
CREATE TABLE askmate_answer_comment(
comment_ID SERIAL PRIMARY KEY,
answer_id INT REFERENCES answer(answer_id) ON DELETE CASCADE,
comment_message TEXT,
submission_time TIMESTAMP,
edited_number INT,
comment_username TEXT,
userID INT REFERENCES users(userID) ON DELETE CASCADE
);
CREATE TABLE askmate_question_comment(
comment_ID SERIAL PRIMARY KEY,
question_id INT REFERENCES question(question_id) ON DELETE CASCADE,
comment_message TEXT,
submission_time TIMESTAMP,
edited_number INT,
comment_username TEXT,
userID INT REFERENCES users(userID) ON DELETE CASCADE
);

CREATE TABLE question_tag(
question_id INT REFERENCES question(question_id) ON DELETE CASCADE,
tag_id INT REFERENCES tag(tag_id) ON DELETE CASCADE
);


INSERT INTO users(username, user_password, registration_time, email, reputation)
VALUES ('asdasd', 'valami123', '2020-03-22 14:55:01', 'iamking@gmail.com', 0);
INSERT INTO users(username, user_password, registration_time, email, reputation)
VALUES ('xXxmilf_hunter69', 'milforgilf', '2019-02-12 10:53:44', 'mmmmlady13@citromail.hu', 0);
INSERT INTO users(username, user_password, registration_time, email, reputation)
VALUES ('ninja the gamer', 'reportth1splayer', '2020-04-02 08:10:03', 'emayraitgamerz@promail.ro', 0);

INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image, question_username,userID)
VALUES ('2020-03-22 13:32:02', 12, 0, 'What is c#?', 'I see it is simiral to Java but to what extent?', null, 'asdasd',1);
INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image, question_username,userID)
VALUES ('2020-02-18 08:02:11', 5, 0, 'Polymorphs', 'I kinda understand what it is, but whats the point? Could you show me some business level example?', null, 'asdasd',1);
INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image, question_username,userID)
VALUES ('2019-12-24 19:30:12', 42, 0, 'Merry Christmas! But...', 'I dont get how to start the web module so im looking at memes. Any idea?', null, 'xXxmilf_hunter69',2);
INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image, question_username,userID)
VALUES ('2020-03-05 16:45:17', 3, 0, 'Could you help me with inheritence pls...', 'So I have 2 abstact classes but it errors when i make them parents in a child class', null, 'ninja the gamer',3);
INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image, question_username,userID)
VALUES ('2020-01-01 02:10:07', 35, 0, 'Happy New Year!', 'I didnt go anywhere to party. Anyone up to have some pairprogramming?', null, 'xXxmilf_hunter69',2);
INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image, question_username,userID)
VALUES ('2020-03-22 14:55:01', 0, 0, 'Game development?', 'Which language is better and why? Please I want a normal discussion, not arguing!!', null, 'xXxmilf_hunter69',2);

INSERT INTO answer(submission_time, vote_number, question_id, answer_message, answer_image, answer_username,userID)
VALUES ('2020-01-02 12:45:03', 8, 5, 'You too, mate. Guess im late to the party thou, i went out and got wasted.', null, 'asdasd',1);
INSERT INTO answer(submission_time, vote_number, question_id, answer_message, answer_image, answer_username,userID)
VALUES ('2020-01-02 13:10:33', 3, 5, 'Get well friend', null, 'xXxmilf_hunter69',2);
INSERT INTO answer(submission_time, vote_number, question_id, answer_message, answer_image, answer_username,userID)
VALUES ('2019-12-24 20:15:44', 2, 3, 'Its already a good start by wanting to learn at Christmas Eve. Now seriously, spend the upcoming days with your family and we are gonna help you tomorrow!', null, 'ninja the gamer',3);
INSERT INTO answer(submission_time, vote_number, question_id, answer_message, answer_image, answer_username,userID)
VALUES ('2020-03-23 07:32:56', 17, 1, 'I would say there is about a 80% similarity, but as time goes on the gap gets wider. C# for Dummies is a nice book, give that a try.', null, 'xXxmilf_hunter69',2);
INSERT INTO answer(submission_time, vote_number, question_id, answer_message, answer_image, answer_username,userID)
VALUES ('2020-01-02 12:45:03', 12, 1, 'Original answer is great, i would like to add a little to it: C# seems a bit more modern while Java has more support online!', null, 'asdasd',1);

INSERT INTO askmate_question_comment(question_id,comment_message, submission_time, edited_number,comment_username,userID)
VALUES (3,'+1 to this answer. Get some nice rest and have a great time! A fresh mind is always more succesfull than a stressed one.', '2019-12-25 08:22:57', 0,'asdasd',1);

INSERT INTO tag(tag_name) VALUES ('JavaVSC#');
INSERT INTO tag(tag_name) VALUES ('OOP');
INSERT INTO tag(tag_name) VALUES ('ASP');
INSERT INTO tag(tag_name) VALUES ('.NET');
INSERT INTO tag(tag_name) VALUES ('Offtopic');
INSERT INTO tag(tag_name) VALUES ('GameDev');

INSERT INTO question_tag(question_id, tag_id) VALUES (1, 1);
INSERT INTO question_tag(question_id, tag_id) VALUES (2, 2);
INSERT INTO question_tag(question_id, tag_id) VALUES (3, 3);
INSERT INTO question_tag(question_id, tag_id) VALUES (4, 4);
INSERT INTO question_tag(question_id, tag_id) VALUES (5, 5);
INSERT INTO question_tag(question_id, tag_id) VALUES (6, 6);


