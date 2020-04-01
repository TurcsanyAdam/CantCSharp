DROP TABLE IF EXISTS question_tag;
DROP TABLE IF EXISTS tag;
DROP TABLE IF EXISTS askmate_question_comment;
DROP TABLE IF EXISTS askmate_answer_comment;
DROP TABLE IF EXISTS answer;
DROP TABLE IF EXISTS question;

CREATE TABLE question(
question_id SERIAL PRIMARY KEY,
submission_time TIMESTAMP,
view_number INT,
vote_number INT,
question_title TEXT,
question_message TEXT,
question_image TEXT
);
CREATE TABLE answer(
answer_id SERIAL PRIMARY KEY,
submission_time TIMESTAMP,
vote_number INT,
question_id INT REFERENCES question(question_id) ON DELETE CASCADE,
answer_message TEXT,
answer_image TEXT	
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
edited_number INT
);
CREATE TABLE askmate_question_comment(
comment_ID SERIAL PRIMARY KEY,
question_id INT REFERENCES question(question_id) ON DELETE CASCADE,
comment_message TEXT,
submission_time TIMESTAMP,
edited_number INT
);

CREATE TABLE question_tag(
question_id INT REFERENCES question(question_id) ON DELETE CASCADE,
tag_id INT REFERENCES tag(tag_id) ON DELETE CASCADE
);

INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image)
VALUES ('2020-03-22 13:32:02', 12, 0, 'What is c#?', 'I see it is simiral to Java but to what extent?', null);
INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image)
VALUES ('2020-02-18 08:02:11', 5, 0, 'Polymorphs', 'I kinda understand what it is, but whats the point? Could you show me some business level example?', null);
INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image)
VALUES ('2019-12-24 19:30:12', 42, 0, 'Merry Christmas! But...', 'I dont get how to start the web module so im looking at memes. Any idea?', null);
INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image)
VALUES ('2020-03-05 16:45:17', 3, 0, 'Could you help me with inheritence pls...', 'So I have 2 abstact classes but it errors when i make them parents in a child class', null);
INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image)
VALUES ('2020-01-01 02:10:07', 35, 0, 'Happy New Year!', 'I didnt go anywhere to party. Anyone up to have some pairprogramming?', null);
INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image)
VALUES ('2020-03-22 14:55:01', 0, 0, 'Game development?', 'Which language is better and why? Please I want a normal discussion, not arguing!!', null);

INSERT INTO answer(submission_time, vote_number, question_id, answer_message, answer_image)
VALUES ('2020-01-02 12:45:03', 8, 5, 'You too, mate. Guess im late to the party thou, i went out and got wasted.', null);
INSERT INTO answer(submission_time, vote_number, question_id, answer_message, answer_image)
VALUES ('2020-01-02 13:10:33', 3, 5, 'Get well friend', null);
INSERT INTO answer(submission_time, vote_number, question_id, answer_message, answer_image)
VALUES ('2019-12-24 20:15:44', 2, 3, 'Its already a good start by wanting to learn at Christmas Eve. Now seriously, spend the upcoming days with your family and we are gonna help you tomorrow!', null);
INSERT INTO answer(submission_time, vote_number, question_id, answer_message, answer_image)
VALUES ('2020-03-23 07:32:56', 17, 1, 'I would say there is about a 80% similarity, but as time goes on the gap gets wider. C# for Dummies is a nice book, give that a try.', null);
INSERT INTO answer(submission_time, vote_number, question_id, answer_message, answer_image)
VALUES ('2020-01-02 12:45:03', 12, 1, 'Original answer is great, i would like to add a little to it: C# seems a bit more modern while Java has more support online!', null);

INSERT INTO askmate_question_comment(question_id,comment_message, submission_time, edited_number)
VALUES (3,'+1 to this answer. Get some nice rest and have a great time! A fresh mind is always more succesfull than a stressed one.', '2019-12-25 08:22:57', 0);

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