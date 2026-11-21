import ActContextButton from '../../features/context/components/buttons/act_context_button/ActContextButton';
import { formatDuration } from '../../utils/formatDuration/formatDuration';
import { highlightWord } from '../../utils/highlight_word/highlightWord';
import EllipsisText from '../ellipsis_text/EllipsisText';

import styles from '../source/source.module.css';

function Source({youtubeVideoTitle, context, duration, word, onNavigateToPlayer, onDelete}){

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
                <ActContextButton type="delete" onClick={onDelete}/>
            </div>
        </div>
    )
}

export default Source;