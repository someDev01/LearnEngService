
import { highlightWord } from '../../utils/highlight_word/highlightWord';
import styles from '../examples/examples.module.css';
import WordPronunciation from '../word_pronunciation/WordPronunciation';

function Examples({ examples = [], word }) {
    return (
        <div className={styles.block_examples}>
            <p style={{margin: 0, color: '#6c5a51'}}>Примеры</p>

            {examples.map((ex, index) => (
                <div className={styles.wrapper} style={{display:'flex', width:'100%', gap:'4px'}} key={index}>
                    <div className={styles.note_text}>
                        <p>
                            {highlightWord(ex.text, word, styles.highlight)}
                        </p>
                        <p className={styles.text_translate}> 
                            {ex.translate}
                        </p>
                    </div>
                    <WordPronunciation word={ex.text}/>
                </div>
            ))}
        </div>
    );
}

export default Examples;