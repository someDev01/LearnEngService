import styles from '../question_translation/question_translation.module.css';

function QuestionTranslation({translation}){
    return(
        <div className={styles.question_translation}>
            <p>{translation}</p>
        </div>
    )
}

export default QuestionTranslation;