import styles from '../training_next_button/training_next_button.module.css';

function TrainingNextButton({onGoNext}){
    return(
        <button className={styles.training_next_button} onClick={onGoNext}>Дальше</button>
    )
}

export default TrainingNextButton;