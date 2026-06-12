import { useRef, useState } from 'react';
import styles from '../word_pronunciation/word_pronunciation.module.css';
import { Volume2Icon } from 'lucide-react';

function WordPronunciation({word}){

    const [rate, setRate] = useState(1);
    const rateRef = useRef(1);

    const speakWord = () => {
        window.speechSynthesis.cancel();

        const utterance = new SpeechSynthesisUtterance(word);
        utterance.lang = 'en-US';      
        utterance.rate = rateRef.current;               
        utterance.pitch = 1;                
        utterance.volume = 1;               
        
        window.speechSynthesis.speak(utterance);
        
    };

    const toggleRate = () => {
        const newRate = rateRef.current === 1 ? 0.5 : rateRef.current === 0.5 ? 0.25 : 1;
        rateRef.current = newRate;
        setRate(newRate);
    }

    return(
        <div className={styles.speech_block}>
            <div className={styles.button_pronunciation} onClick={speakWord}>
                <Volume2Icon size={18} color='#b4b4b4'/>
            </div>
            <div className={styles.speech_rate} onClick={toggleRate}>
                <p>x{rate}</p>
            </div>
        </div>
    )
}

export default WordPronunciation;