import styles from '../option/option.module.css';
import Transcription from '../transcription/Transcription';

function Option({word, onClick, isShowResult, style}){
    return(
        <div className={styles.block_option}>
            <button className={styles.option} onClick={onClick} disabled={isShowResult} style={style}>
                {word}
            </button>
            <Transcription word={word}/>
        </div>
    )
}

export default Option;