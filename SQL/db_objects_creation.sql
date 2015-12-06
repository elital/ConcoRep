/* WORDS */

create table WORDS
(
	ID          number(9) not null,
	WORD        varchar2(30) not null,
	REPETITION  number(9) not null,
  constraint WORDS_PK primary key (ID)
);

create unique index WORDS_U on WORDS(WORD);

create sequence WORDS_S
	start with 1
	increment by 1
	minvalue 1
	maxvalue 999999999
	cycle;

/* RELATIONS */

create table RELATIONS
(
	ID              number(9) not null,
  NAME            varchar2(50) not null,
	FIRST_WORD_ID   number(9) not null,
	SECOND_WORD_ID  number(9) not null,
  constraint RELATIONS_PK primary key (ID),
  constraint RELATIONS_WORDS_FIRST_FK foreign key (FIRST_WORD_ID) references WORDS(ID),
  constraint RELATIONS_WORDS_SECOND_FK foreign key (SECOND_WORD_ID) references WORDS(ID)
);

create unique index RELATIONS_U on RELATIONS(NAME, FIRST_WORD_ID, SECOND_WORD_ID);

create sequence RELATIONS_S
	start with 1
	increment by 1
	minvalue 1
	maxvalue 999999999
	cycle;

/* PHRASES */

create table PHRASES
(
	ID	            number(9) not null,
	PHRASE_NUMBER	  number(9) not null,
	WORD_SEQUENCE	  number(9) not null,
	WORD_ID	        number(9) not null,
  constraint PHRASES_PK primary key (ID),
  constraint PHRASES_WORDS_WORD_ID_FK foreign key (WORD_ID) references WORDS(ID)
);

create unique index PHRASES_U on PHRASES(PHRASE_NUMBER, WORD_SEQUENCE);

create sequence PHRASES_S
	start with 1
	increment by 1
	minvalue 1
	maxvalue 999999999
	cycle;

create sequence PHRASE_NUMBER_S
	start with 1
	increment by 1
	minvalue 1
	maxvalue 999999999
	cycle;

/* WORD_GROUPS */

create table WORD_GROUPS
(
	ID	        number(9) not null,
	GROUP_NAME	varchar2(50) not null,
	WORD_ID	    number(9) not null,
  constraint WORD_GROUPS_PK primary key (ID),
  constraint WORD_GROUPS_WORDS_WORD_ID_FK foreign key (WORD_ID) references WORDS(ID)
);

create unique index WORD_GROUPS_U on WORD_GROUPS(GROUP_NAME, WORD_ID);

create sequence WORD_GROUPS_S
	start with 1
	increment by 1
	minvalue 1
	maxvalue 999999999
	cycle;

/* SONGS */

create table SONGS
(
	ID	          number(9) not null,
	TITLE	        varchar2(50) not null,
	AUTHOR	      varchar2(50) not null,
	PUBLISH_DATE	date,
	ALBUM_NAME	  varchar2(50),
  constraint SONGS_PK primary key (ID)
);

create unique index SONGS_U on SONGS(TITLE, AUTHOR);

create sequence SONGS_S
	start with 1
	increment by 1
	minvalue 1
	maxvalue 999999999
	cycle;

/* SONG_WORDS */

create table SONG_WORDS
(
	ID	          number(9) not null,
	SONG_ID	      number(9) not null,
	WORD_LINE	    number(3) not null,
	WORD_COLUMN   number(3) not null,
	WORD_ID	      number(9) not null,
  constraint SONG_WORDS_PK primary key (ID),
  constraint SONG_WORDS_SONGS_SONG_ID_FK foreign key (SONG_ID) references SONGS(ID),
  constraint SONG_WORDS_WORDS_WORD_ID_FK foreign key (WORD_ID) references WORDS(ID)
);

create unique index SONG_WORDS_U on SONG_WORDS(SONG_ID, WORD_LINE, WORD_COLUMN);

create sequence SONG_WORDS_S
	start with 1
	increment by 1
	minvalue 1
	maxvalue 999999999
	cycle;
