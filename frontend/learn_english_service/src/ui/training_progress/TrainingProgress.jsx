import styles from '../training_progress/training_progress.module.css';

function TrainingProgress({currentIndex, notes}){
    return(
        <div className={styles.progress}>
            <p>{currentIndex + 1} / {notes.length}</p>
        </div>
    )
}
export default TrainingProgress;