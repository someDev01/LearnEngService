import { BookMarkedIcon, Brain, ChevronDown, ChevronUpIcon, Clapperboard, Headphones, Play, SubtitlesIcon } from 'lucide-react';
import styles from './opportunity.module.css';

function Opportunity({index, title, type, description}){
    return(
        <article className={styles.opportunity_card}>
            <div className={`${styles.icon} ${styles[type]}`}>
                {type === "clip" ? 
                    <Play size={24} color='#ff2020'/> :
                    type === "dict" ?
                    <BookMarkedIcon size={24} color='#ff6720'/> :
                    <Headphones size={24} color='#b0fd22'/> }
            </div>
            <header className={styles.title}>
                {title}
            </header>
            <div className={styles.description}>
                {description}
            </div>
        </article>
    )
}

export default Opportunity;