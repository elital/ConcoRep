create or replace package ET_UTILS AS 
  
  function GET_TRUE_RESULT return varchar2;
  
  function GET_FALSE_RESULT return varchar2;
  
  function CONTAINS_LIKE_TEXT(p_song_id in number, p_phrase in varchar2) return varchar2;
  
  function CONTAINS_EXACT_TEXT(p_song_id in number, p_phrase in varchar2) return varchar2;
  
  function CONTAINS_PAIR_IN_LINE(p_song_id in number, p_first_word in varchar2, p_second_word in varchar2) return varchar2;
  
  function CONTAINS_PHRASE(p_song_id in number, p_word_line in number, p_word_column in number, p_phrase_number in number) return varchar2;
  
end ET_UTILS;
