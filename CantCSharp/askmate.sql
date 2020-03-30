DROP TABLE IF EXISTS question_tag;
DROP TABLE IF EXISTS tag;
DROP TABLE IF EXISTS askmate_comment;
DROP TABLE IF EXISTS answer;
DROP TABLE IF EXISTS question;

CREATE TABLE question(
question_id SERIAL PRIMARY KEY,
submission_time DATE,
view_number INT,
vote_number INT,
question_title TEXT,
question_message TEXT,
question_image TEXT
);
CREATE TABLE answer(
answer_id SERIAL PRIMARY KEY,
submission_time DATE,
vote_number INT,
question_id INT REFERENCES question(question_id),
answer_message TEXT,
answer_image TEXT	
);
CREATE TABLE tag(
tag_id SERIAL PRIMARY KEY,
tag_name TEXT
);
CREATE TABLE askmate_comment(
comment_ID SERIAL PRIMARY KEY,
question_id INT REFERENCES question(question_id),
answer_id INT REFERENCES answer(answer_id),
comment_message TEXT,
submission_time DATE,
edited_number INT
);
CREATE TABLE question_tag(
question_id INT REFERENCES question(question_id),
tag_id INT REFERENCES tag(tag_id)
);
