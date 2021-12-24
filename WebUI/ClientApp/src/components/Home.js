import React, { useEffect, useState } from 'react';

const Home = () => {

    const [loading, setLoading] = useState(true);
    const [recipes, setRecipes] = useState([]);

    useEffect(() => {
        fetch('api/RecipeCost')
            .then(result => result.json())
            .then(result => {
                setLoading(false);
                setRecipes(result)
            })
            .catch(error => {
                setLoading(false);
                console.log(error);
            })
    }, [])

    return (
        <div className="row">
            {
                loading ?
                    <div className="spinner-border text-warning" role="status">
                        <span className="sr-only">Loading...</span>
                    </div>
                    :
                    recipes.map((recipe, i) => {
                        return (
                            <div key={recipe.recipe.recipeId} className="col-md-4 col-sm-12" style={{ padding: "1rem" }}>
                                <div className="card" style={{ width: "100%", height: "100%" }}>
                                    <h5 className="card-header">{recipe.recipe.name}</h5>
                                    <div className="card-body">
                                        <p className="card-text">Ingredients</p>
                                        <ul>
                                            {
                                                recipe.recipe.recipeProducts.map((product, j) => {
                                                    return <li key={recipe.recipe.recipeId + " " + product.productId}>{product.product.name}</li>;
                                                })
                                            }
                                        </ul>
                                    </div>
                                    <ul className="list-group list-group-flush">
                                        <li className="list-group-item">Tax: {recipe.saleTaxFormatted}</li>
                                        <li className="list-group-item">Discount: {recipe.wellnessDiscountFormatted}</li>
                                        <li className="list-group-item">Total: {recipe.totalFormatted}</li>
                                    </ul>
                                </div>
                            </div>
                        )
                    })
            }
        </div>
    );
}

export { Home };
