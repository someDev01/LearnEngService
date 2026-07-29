import { BookOpenText, LetterTextIcon, PlayCircle } from 'lucide-react';
import styles from '../profile_progress/profile_progress.module.css';
import AppIcon from '../app_icon/AppIcon';

function ProfileProgress({notesCount, videosCount, englishLevel}){

    const cards = [
        {
            Icon: BookOpenText,
            value: notesCount,
            title: 'Слов',
        },
        {
            Icon: PlayCircle,
            value: videosCount,
            title: 'Видео',
        },
        {
            Icon: LetterTextIcon,
            value: englishLevel,
            title: 'Уровень словаря',
        }
    ];
    return(
        <section className={styles.profile_progress}>
            {cards.map(card => (
                <article 
                    key={card.title}
                    className={styles.card_progress}
                >
                    <AppIcon Icon={card.Icon} className='icon' color='#ff7300'/>
                    <span className={styles.value}>{card.value}</span>
                    <span className={styles.name}>{card.title}</span>
                </article>
            ))}
        </section>
    )
}

export default ProfileProgress