import styles from '../createdat_note/createdat_note.module.css';

function CreatedAtNote({createdAt}){

    const date = new Date(createdAt);

    const formatted = new Intl.DateTimeFormat("sv", {
        year: "numeric",
        month: "2-digit",
        day: "2-digit"
    }).format(date);

    return(
        <p className={styles.createdAt}>{formatted}</p>
    )
}

export default CreatedAtNote;