import { useSpeech } from '../../utils/speesh/useSpeesh';
import styles from '../word_pronunciation/word_pronunciation.module.css';
import { Volume2Icon } from 'lucide-react';

function WordPronunciation({word}){

    const {speak, toggleRate, rate} = useSpeech(word);

    return(
        <div className={styles.speech_block}>
            <div className={styles.button_pronunciation} onClick={speak}>
                <Volume2Icon size={18} color='#b4b4b4'/>
            </div>
            <div className={styles.speech_rate} onClick={toggleRate}>
                <p>x{rate}</p>
            </div>
        </div>
    )
}

export default WordPronunciation;