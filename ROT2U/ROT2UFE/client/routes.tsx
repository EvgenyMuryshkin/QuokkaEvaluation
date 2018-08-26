import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import LED from './components/LED';

export const routes = <Layout>
    <Route path='/' component={LED} />
</Layout>;