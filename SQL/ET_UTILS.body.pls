create or replace package body ET_UTILS as
  
  C_TRUE_RESULT constant varchar2(5) := 'TRUE';
  C_FALSE_RESULT constant varchar2(5) := 'FALSE';
  
  function CONTAINS_TEXT(p_song_id in number, p_phrase in varchar2, p_exact in boolean) return varchar2 is
  
    cursor lyrics_crs is
      select W.WORD
      from   SONG_WORDS SW
           , WORDS      W
      where  SW.SONG_ID = p_song_id
      and    SW.WORD_ID = W.ID
      order by SW.WORD_LINE, SW.WORD_COLUMN;
    
    type words_t is table of WORDS.WORD%type index by pls_integer;
    
    lyrics_row        lyrics_crs%rowtype;
    l_words           words_t;
    l_phrase_leftover varchar2(4000) := trim(p_phrase);
    l_current_index   number := 1;
    l_list_size       number := 0;
    
  begin
    
    if (l_phrase_leftover is null) then
      return C_FALSE_RESULT;
    end if;
    
    loop
      
      if (INSTR(l_phrase_leftover, ' ') = 0) then
        l_words(l_current_index) := l_phrase_leftover;
        exit;
      end if;
      
      l_words(l_current_index) := trim(substr(l_phrase_leftover, 1, instr(l_phrase_leftover, ' ') - 1));
      l_phrase_leftover := trim(substr(l_phrase_leftover, instr(l_phrase_leftover, ' ') + 1));
      l_current_index := l_current_index + 1;
      
    end loop;
    
    l_list_size := l_current_index;
    l_current_index := 1;
    open lyrics_crs;
    
    loop
      
      fetch lyrics_crs into lyrics_row;
      exit when lyrics_crs%notfound;
      
      if (((p_exact) and (lyrics_row.WORD = l_words(l_current_index))) or
          ((not p_exact) and (lyrics_row.WORD like '%' || l_words(l_current_index) || '%'))) then
        
        if (l_current_index = l_list_size) then
          
          close lyrics_crs;
          return C_TRUE_RESULT;
          
        end if;
        
        l_current_index := l_current_index + 1;
        
      else
        
        l_current_index := 1;
        
      end if;
      
    end loop;
    
    return C_FALSE_RESULT;
    
  exception
    when OTHERS then
      
      if (lyrics_crs%isopen) then
        close lyrics_crs;
      end if;
      
      return C_FALSE_RESULT;
      
  end CONTAINS_TEXT;
  
  function GET_TRUE_RESULT return varchar2 is
  begin
    return C_TRUE_RESULT;
  end GET_TRUE_RESULT;
  
  function GET_FALSE_RESULT return varchar2 is
  begin
    return C_FALSE_RESULT;
  end GET_FALSE_RESULT;
  
  function CONTAINS_LIKE_TEXT(p_song_id in number, p_phrase in varchar2) return varchar2 is
  begin
    return CONTAINS_TEXT(p_song_id, p_phrase, true);
  end CONTAINS_LIKE_TEXT;
  
  function CONTAINS_EXACT_TEXT(p_song_id in number, p_phrase in varchar2) return varchar2 is
  begin
    return CONTAINS_TEXT(p_song_id, p_phrase, true);
  end CONTAINS_EXACT_TEXT;
  
  function CONTAINS_PAIR_IN_LINE(p_song_id in number, p_first_word in varchar2, p_second_word in varchar2) return varchar2 is
    
    cursor lyrics_crs is
      select /*W.ID     WORD_ID       --  parameter as id or word ?????????????????
           , */W.WORD
           , SW.WORD_LINE
      from   SONG_WORDS SW
           , WORDS      W
      where  SW.SONG_ID = p_song_id
      and    SW.WORD_ID = W.ID
      order by SW.WORD_LINE, SW.WORD_COLUMN;
    
    lyrics_row        lyrics_crs%rowtype;
    l_first_word      WORDS.WORD%type := trim(p_first_word);
    l_second_word     WORDS.WORD%type := trim(p_second_word);
    l_first_found     number := 0;
    l_second_found    number := 0;
    
  begin
    
    if ((l_first_word is null) or (l_second_word is null)) then
      return C_FALSE_RESULT;
    end if;
    
    open lyrics_crs;
    
    loop
      
      fetch lyrics_crs into lyrics_row;
      exit when lyrics_crs%notfound;
      
      if (l_first_word = lyrics_row.WORD) then
        
        if (l_second_found = lyrics_row.WORD_LINE) then
          
          return C_TRUE_RESULT;
          
        elsif (l_first_found != lyrics_row.WORD_LINE) then
          
          l_first_found := lyrics_row.WORD_LINE;
          continue;
          
        end if;
        
      end if;
      
      if (l_second_word = lyrics_row.WORD) then
        
        if (l_first_found = lyrics_row.WORD_LINE) then
          
          return C_TRUE_RESULT;
          
        else
          
          l_second_found := lyrics_row.WORD_LINE;
          
        end if;
        
      end if;
      
    end loop;
    
    return C_FALSE_RESULT;
    
  exception
    when OTHERS then
      
      if (lyrics_crs%isopen) then
        close lyrics_crs;
      end if;
      
      return C_FALSE_RESULT;
      
  end CONTAINS_PAIR_IN_LINE;
  
  function CONTAINS_PHRASE(p_song_id in number, p_word_line in number, p_word_column in number, p_phrase_number in number) return varchar2 is
    
    cursor phrase_crs is
      select P.WORD_ID
      from   PHRASES P
      where  P.PHRASE_NUMBER = p_phrase_number
      order by P.WORD_SEQUENCE;
      
    cursor lyrics_crs is
      select SW.WORD_ID
      from   SONG_WORDS SW
      where  SW.SONG_ID = p_song_id
      and    (((SW.WORD_LINE = p_word_line) and (SW.WORD_COLUMN >= p_word_column)) or (SW.WORD_LINE > p_word_line))
      order by SW.WORD_LINE, SW.WORD_COLUMN;
    
    phrase_row        phrase_crs%rowtype;
    lyrics_row        lyrics_crs%rowtype;
    
  begin
    
    open phrase_crs;
    open lyrics_crs;
    
    loop
      
      fetch phrase_crs into phrase_row;
      fetch lyrics_crs into lyrics_row;
      
      if (phrase_crs%notfound) then
        
        close phrase_crs;
        close lyrics_crs;
        
        return C_TRUE_RESULT;
        
      end if;
      
      if (lyrics_crs%notfound) then
        
        close phrase_crs;
        close lyrics_crs;
        
        return C_FALSE_RESULT;
        
      end if;
      
      if (phrase_row.WORD_ID != lyrics_row.WORD_ID) then
        
        close phrase_crs;
        close lyrics_crs;
        
        return C_FALSE_RESULT;
        
      end if;
      
    end loop;
    
    return C_FALSE_RESULT;
    
  exception
    when OTHERS then
      
      if (lyrics_crs%isopen) then
        close lyrics_crs;
      end if;
      
      if (phrase_crs%isopen) then
        close phrase_crs;
      end if;
      
      return C_FALSE_RESULT;
      
  end CONTAINS_PHRASE;
  
end ET_UTILS;
