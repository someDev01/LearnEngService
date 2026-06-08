import { CheckCircle, GraduationCap, Target } from 'lucide-react';
import styles from '../no_notes/no_notes.module.css';

function NoNotes(){
    return(
        <div className={styles.block_no_notes}>
            <p className={styles.title}>Пока нет заметок для тренировки!</p>
            <div className={styles.target}>
                <p>Добавьте слова из коротких видео и мы сразу начнем</p>
                <GraduationCap size={28} color='#ffa06c'/>
            </div>
        </div>
    )
}

export default NoNotes;