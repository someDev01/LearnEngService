import { highlightWord } from '../../utils/highlight_word/highlightWord';
import styles from '../ellipsis_text/ellipsis_text.module.css';

function EllipsisText({youtubeVideoTitle, word, onClick}){
    return(
        <div className={styles.block_text} onClick={onClick}>
            <p>
                {highlightWord(
                    `${youtubeVideoTitle}`, 
                    word,
                    styles.highlight
                )}
            </p>
        </div>
    )
}

export default EllipsisText;