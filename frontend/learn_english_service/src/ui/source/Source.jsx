import { formatDuration } from '../../utils/formatDuration/formatDuration';
import { highlightWord } from '../../utils/highlight_word/highlightWord';
import EllipsisText from '../ellipsis_text/EllipsisText';

import styles from '../source/source.module.css';

function Source({youtubeVideoTitle, context, duration, word, onNavigateToPlayer}){

    const time = formatDuration(duration || {});

    return(
        <div className={styles.source}>
            <div className={styles.source_header}>        
                <EllipsisText 
                    youtubeVideoTitle={youtubeVideoTitle} 
                    word={word}
                    onClick={onNavigateToPlayer}
                />
                {duration && <div className={styles.duration_source}>
                    <p>{time}</p>
                </div>}
            </div>

            <div className={styles.source_context}>
                <p>
                    {highlightWord(context, word, styles.highlight)}
                </p>
            </div>
        </div>
    )
}

export default Source;