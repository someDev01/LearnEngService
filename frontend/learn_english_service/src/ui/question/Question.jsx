import styles from '../question/question.module.css';

function Question({question}){
    return(
        <div className={styles.question}>
            <p>{question}</p>
        </div>
    )
}

export default Question;