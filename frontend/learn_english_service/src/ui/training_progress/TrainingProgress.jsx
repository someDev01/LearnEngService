import styles from '../training_progress/training_progress.module.css';

function TrainingProgress({currentIndex, trainingWords}){
    return(
        <div className={styles.progress}>
            <p>{currentIndex + 1} / {trainingWords}</p>
        </div>
    )
}
export default TrainingProgress;