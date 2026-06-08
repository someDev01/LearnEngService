import { BookMarkedIcon, Brain, Clapperboard, SubtitlesIcon } from 'lucide-react';
import styles from '../oppotrunity_card/opportunity_card.module.css';

function OpportunityCard({title, description, type}){
    return(
        <div className={styles.opportunity}>
            <div className={styles.icon}>
                {type === "videos" ? 
                    <Clapperboard size={26} color='#ff7f1e'/> :
                    type === "subs" ? 
                    <SubtitlesIcon size={26} color='#ff7f1e'/> : 
                    type === "disc" ?
                     <BookMarkedIcon size={26} color='#ff7f1e'/> :
                     <Brain size={26} color='#ff7f1e'/> }
            </div>
            <div className={styles.title}>
                <p>{title}</p>
                <div className={styles.description}>
                    <p>{description}</p>
                </div>
            </div>
        </div>
    )
}

export default OpportunityCard;