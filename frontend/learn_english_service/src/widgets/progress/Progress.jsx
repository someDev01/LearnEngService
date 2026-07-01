import { use } from 'react';
import styles from '../progress/progress.module.css';
import ProgressCart from '../../ui/progress_cart/ProgressCart';

function Progress({userNotes}){

    const todayDate = new Date().toDateString();    
    
    const addedToday = userNotes.filter((note) => {   
        const noteDateCreate = new Date(note.createdAt); 
        return noteDateCreate.toDateString() === todayDate;
    }).length;

    const learnedToday = userNotes.filter((note) => {
        const noteDateLastTraining = new Date(note?.lastTrainedAt);        
        
        return noteDateLastTraining.toDateString() === todayDate;
    }).length;       

    return(
        <div className={styles.progress__container}>
            <p>Прогресс за сегодня: </p>
            <div className={styles.cards}>
                <ProgressCart count={addedToday} text="Добавлено слов" type="added"/>
                <ProgressCart count={learnedToday} text="Изучено слов" type="learned"/>
            </div>
        </div>
    )
}

export default Progress;