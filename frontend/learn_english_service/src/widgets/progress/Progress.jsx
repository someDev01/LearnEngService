import { use } from 'react';
import styles from '../progress/progress.module.css';
import ProgressCart from '../../ui/progress_cart/ProgressCart';

function Progress({addedCount, trainedCount}){

    return(
        <div className={styles.progress__container}>
            <p>Прогресс за сегодня: </p>
            <div className={styles.cards}>
                <ProgressCart count={addedCount} text="Добавлено слов" type="added"/>
                <ProgressCart count={trainedCount} text="Изучено слов" type="learned"/>
            </div>
        </div>
    )
}

export default Progress;