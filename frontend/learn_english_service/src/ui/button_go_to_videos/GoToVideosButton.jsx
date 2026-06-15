import { ArrowBigRight } from 'lucide-react';
import styles from '../button_go_to_videos/go_to_videos_button.module.css';

function GoToVideosButton({onClick}){
    return(
        <>
            <button className={styles.go_to} onClick={onClick}>
                Перейти  к видео
                <ArrowBigRight size={20} color='rgb(255, 129, 75)'/>
            </button>
        </>
    )
}

export default GoToVideosButton;