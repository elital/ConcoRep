drop table RELATIONS;

drop index RELATIONS_U;

drop sequence RELATIONS_S;

drop table PHRASES;

drop index PHRASES_U;

drop sequence PHRASES_S;

drop sequence PHRASE_NUMBER_S;

drop table WORD_GROUPS;

drop index WORD_GROUPS_U;

drop sequence WORD_GROUPS_S;

drop table SONG_WORDS;

drop index SONG_WORDS_U;

drop sequence SONG_WORDS_S;

drop table SONGS;

drop index SONGS_U;

drop sequence SONGS_S;

drop table WORDS;

drop index WORDS_U;

drop sequence WORDS_S;

select * from user_objects;
select * from RECYCLEBIN;
purge RECYCLEBIN;
