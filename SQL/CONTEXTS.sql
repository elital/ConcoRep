create or replace view CONTEXTS is
select S.ID             SONG_ID
     , S.TITLE          SONG_TITLE
     , S.AUTHOR         SONG_AUTHOR
     , S.PUBLISH_DATE   SONG_PUBLISH_DATE
     , S.ALBUM_NAME     SONG_ALBUM_NAME
     , SW.WORD_LINE     MATCH_LINE
     , SW.WORD_COLUMN   MATCH_COLUMN
     , W.WORD           MATCH_WORD
     , W.ID             MATCH_WORD_ID
     , CSW.WORD_LINE    CONTEXT_LINE
     , CSW.WORD_COLUMN  CONTEXT_COL
     , CW.WORD          CONTEXT_WORD
     , S.ID || '-' || SW.WORD_LINE || '-' || SW.WORD_COLUMN    MATCH_LOCATION
from   SONGS      S
     , SONG_WORDS SW
     , WORDS      W
     , SONG_WORDS CSW
     , WORDS      CW
where  S.ID = SW.SONG_ID
and    SW.WORD_ID = W.ID
and    CSW.SONG_ID = S.ID
and    CSW.WORD_LINE in (SW.WORD_LINE - 1, SW.WORD_LINE, SW.WORD_LINE + 1)
and    CSW.WORD_ID = CW.ID
order by S.ID, SW.WORD_LINE, SW.WORD_COLUMN, CSW.WORD_LINE, CSW.WORD_COLUMN
