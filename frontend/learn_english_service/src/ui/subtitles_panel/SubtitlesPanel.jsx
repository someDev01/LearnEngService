import { useEffect, useRef, useState } from 'react';
import styles from '../subtitles_panel/subtitles_panel.module.css';
import ButtonAddNote from '../button_add/ButtonAddNote';
import { useSelector } from 'react-redux';

function SubtitlesPanel({ 
  isShowedEn, isShowedRu, textEn, textRu, onWordClick, onHasWordInNotes}) {

  const words = (textEn || "").toLowerCase().split(" ");

  const handleWordClick = (word, e) => {
    e.stopPropagation();
    onWordClick?.(word);
  };

  return (
    <div className={styles.subtitles_panel}>
      <div className={styles.track}>
        {isShowedEn && (
          <div className={styles.subtitles_en}>
            {words.map((word, index) => {

              const one = word;
              const prevTwo = index > 0  ? words[index - 1] + " " + word : null;
              const prevThree = index > 1 ? words[index - 2] + " " + words[index - 1] + " " + word : null;
              const two = index < words.length - 1 ? word + " " + words[index + 1] : null;
              const three = index < words.length - 2 ? word + " " + words[index + 1] + " " + words[index + 2] : null;

              const isHighlighted =
                onHasWordInNotes(one) ||
                (two && onHasWordInNotes(two)) ||
                (three && onHasWordInNotes(three)) || 
                (prevTwo && onHasWordInNotes(prevTwo)) || 
                (prevThree && onHasWordInNotes(prevThree));

              return (
                <span
                  className={styles.en}
                  key={index}
                  onClick={(e) => handleWordClick(word, e)}
                  style={{
                    color:
                       isHighlighted
                        ? '#ffc7a2'
                        : 'white',
                  }}
                >
                  {word + " "}
                </span>
              );
            })}
          </div>
        )}

        {isShowedRu && (
          <div className={styles.subtitles_ru}>
            <p>{textRu}</p>
          </div>
        )}
      </div>
    </div>
  );
}

export default SubtitlesPanel;